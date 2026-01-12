# ============================================================================
# Dockerfile pour l'application Neovore.Web (ASP.NET Core 8.0 MVC)
# ============================================================================
# Ce Dockerfile utilise une approche multi-stage pour optimiser la taille
# de l'image finale et améliorer la sécurité.
# ============================================================================

# ----------------------------------------------------------------------------
# ÉTAPE 1 : BUILD
# ----------------------------------------------------------------------------
# Utilise l'image SDK complète qui contient tous les outils nécessaires
# pour compiler et publier l'application (.NET SDK, compilateur, etc.)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Définit le répertoire de travail dans le conteneur
# Toutes les commandes suivantes seront exécutées dans ce répertoire
WORKDIR /src

# ----------------------------------------------------------------------------
# Restauration des dépendances NuGet
# ----------------------------------------------------------------------------
# Copie uniquement le fichier .csproj en premier (optimisation Docker cache)
# Si les dépendances n'ont pas changé, Docker réutilisera le cache de cette étape
COPY ["Neovore.Web.csproj", "./"]

# Restaure les packages NuGet définis dans le fichier .csproj
# Cette étape est mise en cache si le .csproj n'a pas changé
RUN dotnet restore "Neovore.Web.csproj"

# ----------------------------------------------------------------------------
# Copie du code source et compilation
# ----------------------------------------------------------------------------
# Copie tout le reste du code source dans le conteneur
# Le .dockerignore exclut les fichiers inutiles (bin/, obj/, etc.)
COPY . .

# Compile l'application en mode Release
# -c Release : Configuration Release (optimisée, sans debug)
# -o /app/build : Répertoire de sortie pour les fichiers compilés
RUN dotnet build "Neovore.Web.csproj" -c Release -o /app/build

# ----------------------------------------------------------------------------
# ÉTAPE 2 : PUBLISH
# ----------------------------------------------------------------------------
# Utilise l'étape build précédente comme base
FROM build AS publish

# Publie l'application pour la production
# -c Release : Configuration Release
# -o /app/publish : Répertoire de sortie pour les fichiers publiés
# /p:UseAppHost=false : Désactive la génération de l'exécutable natif
#                       (on utilise dotnet pour lancer l'application)
RUN dotnet publish "Neovore.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ----------------------------------------------------------------------------
# ÉTAPE 3 : RUNTIME (Image finale)
# ----------------------------------------------------------------------------
# Utilise l'image runtime ASP.NET Core (plus légère, sans SDK)
# Cette image contient uniquement le runtime .NET nécessaire pour exécuter
# l'application, ce qui réduit considérablement la taille de l'image finale
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

# Définit le répertoire de travail pour l'application
WORKDIR /app

# ----------------------------------------------------------------------------
# Création d'un utilisateur non-root pour la sécurité
# ----------------------------------------------------------------------------
# Crée un groupe système "appuser" (-r = système)
# Crée un utilisateur système "appuser" dans ce groupe (-r = système, -g = groupe)
# Exécuter l'application en tant que root est une faille de sécurité
RUN groupadd -r appuser && useradd -r -g appuser appuser

# ----------------------------------------------------------------------------
# Copie des fichiers publiés depuis l'étape publish
# ----------------------------------------------------------------------------
# Copie uniquement les fichiers nécessaires pour l'exécution depuis l'étape publish
# Cela exclut le code source, les outils de build, etc.
COPY --from=publish /app/publish .

# ----------------------------------------------------------------------------
# Création des dossiers pour les uploads
# ----------------------------------------------------------------------------
# Crée la structure de dossiers nécessaire pour stocker les fichiers uploadés :
# - /app/wwwroot/uploads : Dossier racine des uploads
# - /app/wwwroot/uploads/logo : Logos uploadés via l'admin
# - /app/wwwroot/uploads/images : Images des produits/réalisations
# - /app/wwwroot/uploads/videos : Vidéos avec sous-titres
# 
# chown -R : Change le propriétaire récursivement pour que appuser puisse
#            écrire dans ces dossiers
RUN mkdir -p /app/wwwroot/uploads && \
    mkdir -p /app/wwwroot/uploads/logo && \
    mkdir -p /app/wwwroot/uploads/images && \
    mkdir -p /app/wwwroot/uploads/videos && \
    chown -R appuser:appuser /app

# ----------------------------------------------------------------------------
# Sécurité : Passage à l'utilisateur non-root
# ----------------------------------------------------------------------------
# Change l'utilisateur actif pour appuser (non-root)
# Toutes les commandes suivantes seront exécutées en tant que appuser
USER appuser

# ----------------------------------------------------------------------------
# Exposition des ports
# ----------------------------------------------------------------------------
# Expose les ports sur lesquels l'application écoutera
# - 8080 : Port HTTP (production)
# - 8081 : Port HTTPS (si configuré, optionnel)
# Note : Ces ports sont exposés mais doivent être mappés dans docker-compose
EXPOSE 8080
EXPOSE 8081

# ----------------------------------------------------------------------------
# Variables d'environnement
# ----------------------------------------------------------------------------
# ASPNETCORE_URLS : Définit les URLs sur lesquelles l'application écoute
#                   http://+:8080 signifie écouter sur toutes les interfaces
#                   sur le port 8080
ENV ASPNETCORE_URLS=http://+:8080

# ASPNETCORE_ENVIRONMENT : Environnement d'exécution
#                          Production = optimisé, pas de pages d'erreur détaillées
ENV ASPNETCORE_ENVIRONMENT=Production

# ----------------------------------------------------------------------------
# Point d'entrée
# ----------------------------------------------------------------------------
# Commande exécutée au démarrage du conteneur
# Lance l'application ASP.NET Core en utilisant dotnet
# Neovore.Web.dll est le fichier principal généré lors du publish
ENTRYPOINT ["dotnet", "Neovore.Web.dll"]

# ============================================================================
# NOTES IMPORTANTES :
# ============================================================================
# 1. Multi-stage build : Réduit la taille de l'image finale de ~1GB à ~200MB
# 2. Utilisateur non-root : Améliore la sécurité en cas de compromission
# 3. Cache Docker : Les dépendances sont restaurées en premier pour optimiser
#    le cache lors des rebuilds
# 4. Volumes : Les uploads doivent être montés en volume pour persister les données
# 5. Migrations : Sont appliquées automatiquement au démarrage via Program.cs
# ============================================================================

