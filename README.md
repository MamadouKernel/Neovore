# NÉOVORE – Site MVC ASP.NET Core 8.0

Site web professionnel pour NÉOVORE - Solutions écologiques & industrielles.

## 🚀 Fonctionnalités

- **Gestion de contenu** : Produits, Services, Réalisations
- **Administration** : Interface d'administration complète
- **Accessibilité** : Support vidéos avec sous-titres et descriptions
- **Analytics** : Suivi des visites et recherches
- **SEO** : Optimisé pour le référencement
- **Responsive** : Design adaptatif mobile/tablette/desktop

## 📋 Prérequis

### Développement local
- .NET SDK 8.0
- PostgreSQL 12+
- Visual Studio 2022 / VS Code / Rider

### Production (Docker)
- Docker Desktop / Docker Engine
- Docker Compose v2.0+

## 🛠️ Installation

### Option 1 : Développement local

1. **Cloner le dépôt**
```bash
git clone https://github.com/MamadouKernel/Neovore.git
cd Neovore/Neovore.Web
```

2. **Configurer la base de données**

Éditer `appsettings.json` :
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=neovore_db;Username=postgres;Password=root"
  }
}
```

3. **Installer les dépendances et lancer**
```bash
dotnet restore
dotnet ef database update
dotnet run
```

L'application sera accessible sur : `https://localhost:5001` ou `http://localhost:5000`

### Option 2 : Docker (Recommandé pour la production)

1. **Construire et démarrer**
```bash
docker-compose up -d
```

2. **Voir les logs**
```bash
docker-compose logs -f
```

L'application sera accessible sur : `http://localhost:8080`

📖 **Documentation Docker complète** : Voir [DOCKER.md](DOCKER.md)

## 🔐 Accès Administration

- **URL** : `/Admin/Account/Login`
- **Identifiants par défaut** :
  - Username : `admin`
  - Password : `Admin@123`

⚠️ **Important** : Changez le mot de passe par défaut en production !

## 📁 Structure du projet

```
Neovore.Web/
├── Areas/Admin/          # Zone d'administration
├── Controllers/          # Contrôleurs MVC
├── Domain/Entities/     # Entités du domaine
├── Infrastructure/      # Services d'infrastructure
├── Application/         # Services métier
├── Views/               # Vues Razor
├── wwwroot/             # Fichiers statiques
└── Migrations/          # Migrations Entity Framework
```

## 🗄️ Base de données

Les migrations sont appliquées automatiquement au démarrage de l'application.

Pour appliquer manuellement :
```bash
dotnet ef database update
```

## 📝 Configuration

### Variables d'environnement importantes

- `ConnectionStrings__DefaultConnection` : Chaîne de connexion PostgreSQL
- `ASPNETCORE_ENVIRONMENT` : Environment (Development, Production)
- `ASPNETCORE_URLS` : URLs d'écoute (défaut: http://+:8080)

### Fichiers de configuration

- `appsettings.json` : Configuration par défaut
- `appsettings.Development.json` : Configuration développement (ignoré par Git)
- `appsettings.Production.json` : Configuration production (ignoré par Git)

## 🎨 Personnalisation

### Logo et nom du site

1. Accéder à `/Admin/Settings`
2. Modifier le nom de l'entreprise
3. Uploader un nouveau logo

Le logo sera automatiquement utilisé dans :
- La navbar
- Le footer
- Le favicon
- Les meta tags Open Graph (réseaux sociaux)

### Statistiques page d'accueil

Modifier les statistiques (années d'expérience, projets réalisés, satisfaction client) dans `/Admin/Settings`.

## 📚 Documentation

- [Guide Docker](DOCKER.md) : Déploiement avec Docker
- [Guide Vidéos Accessibles](GUIDE_VIDEOS_ACCESSIBLES.md) : Ajout de vidéos avec sous-titres
- [Backlog Technique](BACKLOG_TECHNIQUE.md) : Liste des tâches techniques

## 🛡️ Sécurité

- Authentification par cookies
- Hashage des mots de passe avec BCrypt
- Protection CSRF sur tous les formulaires
- Utilisateur non-root dans Docker
- Validation des entrées utilisateur

## 🧪 Technologies utilisées

- **Backend** : ASP.NET Core 8.0 MVC
- **Base de données** : PostgreSQL
- **ORM** : Entity Framework Core 8.0
- **Frontend** : Bootstrap 5.3, Bootstrap Icons
- **Containerisation** : Docker, Docker Compose

## 📄 Licence

Propriétaire - Tous droits réservés

## 👥 Contribution

Ce projet est privé. Pour toute question ou suggestion, contactez l'équipe de développement.

---

**Développé avec ❤️ pour NÉOVORE**
