# Guide de configuration Git pour Neovore

## Commandes pour initialiser le dépôt GitHub

### 1. Initialiser Git (si pas déjà fait)
```bash
git init
```

### 2. Ajouter tous les fichiers
```bash
git add .
```

### 3. Premier commit
```bash
git commit -m "Initial commit: Neovore Web Application"
```

### 4. Renommer la branche principale en 'main'
```bash
git branch -M main
```

### 5. Ajouter le remote GitHub
```bash
git remote add origin https://github.com/MamadouKernel/Neovore.git
```

### 6. Pousser vers GitHub
```bash
git push -u origin main
```

## Commandes complètes (copier-coller)

```bash
git init
git add .
git commit -m "Initial commit: Neovore Web Application"
git branch -M main
git remote add origin https://github.com/MamadouKernel/Neovore.git
git push -u origin main
```

## Fichiers ignorés par Git

Le fichier `.gitignore` exclut automatiquement :
- ✅ Fichiers de build (`bin/`, `obj/`)
- ✅ Fichiers de configuration sensibles (`appsettings.*.json`, `.env`)
- ✅ Fichiers uploadés par les utilisateurs (`wwwroot/uploads/*`)
- ✅ Fichiers temporaires et caches
- ✅ Fichiers IDE (Visual Studio, VS Code, Rider)

## ⚠️ Important : Fichiers sensibles

Les fichiers suivants sont **ignorés** et ne seront **pas** poussés sur GitHub :
- `appsettings.Development.json`
- `appsettings.Production.json`
- `.env`
- `docker-compose.override.yml`
- Tous les fichiers dans `wwwroot/uploads/`

## Commandes utiles après le premier push

### Voir l'état des fichiers
```bash
git status
```

### Ajouter des modifications
```bash
git add .
git commit -m "Description des modifications"
git push
```

### Créer une nouvelle branche
```bash
git checkout -b feature/nom-de-la-feature
git push -u origin feature/nom-de-la-feature
```

### Voir l'historique
```bash
git log --oneline
```

## Structure recommandée des commits

Utilisez des messages de commit clairs :
- `feat: Ajout de la fonctionnalité X`
- `fix: Correction du bug Y`
- `docs: Mise à jour de la documentation`
- `refactor: Refactorisation du code Z`
- `style: Amélioration du style CSS`

## Problèmes courants

### Erreur : "remote origin already exists"
```bash
git remote remove origin
git remote add origin https://github.com/MamadouKernel/Neovore.git
```

### Erreur : "failed to push some refs"
```bash
git pull origin main --rebase
git push -u origin main
```

### Oublier d'ajouter un fichier
```bash
git add nom-du-fichier
git commit --amend --no-edit
git push --force-with-lease
```

