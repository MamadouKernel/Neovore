# Guide de déploiement Docker

Ce guide explique comment construire et déployer l'application Neovore avec Docker.

## Prérequis

- Docker Desktop (Windows/Mac) ou Docker Engine (Linux)
- Docker Compose v2.0+

## Démarrage rapide

### 1. Configuration

Créez un fichier `.env` à la racine du projet (optionnel, pour personnaliser les variables) :

```env
POSTGRES_PASSWORD=votre_mot_de_passe_securise
```

### 2. Construction et démarrage

```bash
# Construire et démarrer tous les services
docker-compose up -d

# Voir les logs
docker-compose logs -f

# Voir les logs d'un service spécifique
docker-compose logs -f web
docker-compose logs -f postgres
```

### 3. Accès à l'application

- **Application web** : http://localhost:8080
- **Base de données PostgreSQL** : localhost:5432
  - Database: `neovore_db`
  - User: `postgres`
  - Password: `root` (ou celui défini dans `.env`)

## Commandes utiles

### Arrêter les services

```bash
docker-compose down
```

### Arrêter et supprimer les volumes (⚠️ supprime les données)

```bash
docker-compose down -v
```

### Reconstruire l'image

```bash
docker-compose build --no-cache
docker-compose up -d
```

### Exécuter des migrations

Les migrations sont appliquées automatiquement au démarrage de l'application via `Program.cs`.

Pour exécuter manuellement :

```bash
# Entrer dans le conteneur
docker-compose exec web bash

# Exécuter les migrations
dotnet ef database update --project . --startup-project .
```

### Accéder à la base de données

```bash
# Via psql dans le conteneur PostgreSQL
docker-compose exec postgres psql -U postgres -d neovore_db
```

## Structure des volumes

- `postgres_data` : Données persistantes de PostgreSQL
- `uploads_data` : Fichiers uploadés (images, vidéos, logos)
- `wwwroot/images` : Images statiques (montées en lecture seule)

## Production

### Variables d'environnement recommandées

Créez un fichier `.env.production` :

```env
POSTGRES_PASSWORD=MotDePasseTresSecurise123!
ASPNETCORE_ENVIRONMENT=Production
```

### Déploiement

1. Construire l'image pour la production :
```bash
docker build -t neovore-web:latest .
```

2. Utiliser docker-compose avec le fichier de production :
```bash
docker-compose -f docker-compose.yml -f docker-compose.prod.yml up -d
```

### Sécurité

- Changez le mot de passe PostgreSQL par défaut
- Utilisez des secrets Docker ou un gestionnaire de secrets
- Configurez HTTPS avec un reverse proxy (nginx, Traefik)
- Limitez l'exposition des ports en production

## Dépannage

### Vérifier l'état des conteneurs

```bash
docker-compose ps
```

### Voir les logs d'erreur

```bash
docker-compose logs web | grep -i error
docker-compose logs postgres | grep -i error
```

### Redémarrer un service

```bash
docker-compose restart web
docker-compose restart postgres
```

### Nettoyer les images inutilisées

```bash
docker system prune -a
```

## Notes importantes

- Les migrations sont appliquées automatiquement au démarrage
- Les fichiers uploadés sont persistés dans le volume `uploads_data`
- Le logo par défaut doit être présent dans `wwwroot/images/logo.jpeg`
- La base de données est initialisée avec des données de seed au premier démarrage

