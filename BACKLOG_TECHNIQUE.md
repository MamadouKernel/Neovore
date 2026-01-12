# Backlog technique – NÉOVORE (Site vitrine + Dashboard Admin) – 1 projet MVC (PostgreSQL)

> Organisation recommandée : 3 sprints (ou 2 si tu charbonnes).  
> Stack : ASP.NET Core MVC .NET 8 + EF Core + PostgreSQL + Auth Cookie (admin).

---

## Sprint 0 – Setup & Fondations
### EPIC: Initialisation
- [ ] Créer le repo Git + conventions (branches, commits)
- [ ] Créer le projet MVC unique + structure dossiers (Domain/Application/Infrastructure)
- [ ] Configurer PostgreSQL + connection string + health-check
- [ ] Installer EF Core + Npgsql + migrations
- [ ] Créer entités + DbContext + indexes uniques sur Slug
- [ ] Seed : SiteSettings + Admin (admin/Admin@123)
- [ ] Pipeline local : `dotnet restore`, `dotnet ef migrations add`, `dotnet run`

Critères d’acceptation
- L’app démarre, DB créée, admin seed OK.

---

## Sprint 1 – Front-office (Vitrine)
### EPIC: Site public
- [ ] Layout global + navigation + footer + responsive
- [ ] Accueil : sections services + réalisations + CTA devis/contact
- [ ] Services : listing + détail (slug)
- [ ] Produits : listing + filtre catégorie + détail (slug)
- [ ] Réalisations : listing + détail + galerie
- [ ] Formulaire Devis : validations + anti-spam + stockage DB + page success
- [ ] Formulaire Contact : validations + anti-spam + stockage DB + page success
- [ ] SEO basique : meta title/description, URLs propres

Critères d’acceptation
- Parcours visiteur complet OK, formulaires persistés en DB.

---

## Sprint 2 – Dashboard Admin (V1)
### EPIC: Auth & Admin
- [ ] Auth cookie : Login/Logout + AccessDenied
- [ ] Dashboard : compteurs (devis, messages, produits, réalisations)
- [ ] CRUD Services (déjà prêt)
- [ ] CRUD Produits (catégorie + images)
- [ ] CRUD Réalisations (images)
- [ ] Demandes : lecture + changement statut (Nouveau/EnCours/Traite)
- [ ] Paramètres site : coordonnées + SEO defaults

Critères d’acceptation
- L’admin peut alimenter le site sans dev.

---

## Sprint 3 – Durcissement (Production-ready)
### EPIC: Sécurité & Qualité
- [ ] Rate limit + logs connexions admin
- [ ] Upload images : validation type/taille, nettoyage, suppression
- [ ] Sitemap.xml + robots.txt
- [ ] Pages légales : mentions + politique confidentialité
- [ ] Accessibilité minimale (alt images, labels form)
- [ ] Déploiement : IIS/Nginx + HTTPS + env vars

Critères d’acceptation
- Déploiement stable + checklist sécurité OK.
