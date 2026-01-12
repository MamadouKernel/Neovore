# Guide : Ajout de vidéos accessibles

## Pour les administrateurs et gestionnaires

Ce guide explique comment ajouter des vidéos avec sous-titres et descriptions pour rendre le site accessible aux personnes sourdes/muettes et mal-voyantes.

## 📋 Étapes pour ajouter une vidéo accessible

### 1. Préparer les fichiers nécessaires

Vous aurez besoin de :
- **Le fichier vidéo** (format MP4, WebM ou OGG)
- **Le fichier de sous-titres** (format .vtt) - **Optionnel mais recommandé**
- **Une description textuelle** de la vidéo

### 2. Créer un fichier de sous-titres (.vtt)

Le fichier VTT (WebVTT) est un format texte simple. Voici un exemple :

```
WEBVTT

00:00:00.000 --> 00:00:05.000
Bienvenue dans cette vidéo de présentation de nos services.

00:00:05.000 --> 00:00:10.000
Nous vous présentons nos solutions écologiques et industrielles.
```

**Conseils :**
- Utilisez un outil en ligne comme [Amara](https://amara.org) ou [Subtitle Edit](https://nikse.dk/subtitleedit) pour créer vos sous-titres
- Ou utilisez les sous-titres automatiques de YouTube (puis exportez en .vtt)

### 3. Ajouter la vidéo dans l'administration

L'interface d'administration vous permettra de :
1. Uploader le fichier vidéo
2. Uploader le fichier de sous-titres (.vtt) - optionnel
3. Remplir une description textuelle de la vidéo

La description textuelle doit décrire ce qui se passe dans la vidéo pour les personnes qui ne peuvent pas la voir.

**Exemple de description :**
> "Cette vidéo de 2 minutes présente notre entreprise NÉOVORE. On y voit notre équipe travaillant sur des installations électriques, puis des images de nos groupes électrogènes en fonctionnement. La vidéo se termine par un plan de notre équipe souriante devant notre entrepôt."

## ✅ Ce qui est fait automatiquement

Une fois que vous avez uploadé les fichiers :
- ✅ La vidéo sera affichée avec les contrôles standard
- ✅ Les sous-titres seront automatiquement intégrés si vous avez uploadé un fichier .vtt
- ✅ La description textuelle sera disponible pour les lecteurs d'écran
- ✅ La vidéo sera responsive (s'adapte à la taille de l'écran)

## 🎯 Exemple concret

**Scénario :** Vous voulez ajouter une vidéo de présentation d'un produit

1. **Préparez vos fichiers :**
   - `presentation-produit.mp4` (votre vidéo)
   - `presentation-produit.vtt` (vos sous-titres)
   - Description : "Vidéo de 3 minutes montrant notre groupe électrogène 20 KVA. On voit l'appareil sous différents angles, puis une démonstration de démarrage, et enfin les témoignages de deux clients satisfaits."

2. **Dans l'interface admin :**
   - Sélectionnez "Vidéo" comme type de média
   - Uploadez `presentation-produit.mp4`
   - Uploadez `presentation-produit.vtt` (optionnel)
   - Collez la description dans le champ texte

3. **C'est tout !** La vidéo sera accessible à tous.

## 📝 Notes importantes

- **Les sous-titres ne sont pas automatiques** - vous devez les créer vous-même
- **La description est obligatoire** pour l'accessibilité
- Les formats vidéo acceptés : MP4, WebM, OGG
- Les formats de sous-titres acceptés : VTT, SRT (recommandé : VTT)

## 🔗 Ressources utiles

- **Créer des sous-titres VTT :** https://amara.org
- **Guide WebVTT :** https://developer.mozilla.org/fr/docs/Web/API/WebVTT_API
- **Validation des sous-titres :** https://quuz.org/webvtt/

