# Projet Fil Rouge : Gestionnaire de Mots de Passe

## Description

Ce projet vise à développer une solution Blazor pour un gestionnaire de mots de passe, destiné aux étudiants en enseignement supérieur. Il permettra aux utilisateurs de stocker, gérer et accéder à leurs mots de passe en toute sécurité grâce à une interface conviviale.

## Installation et lancement

1. Cloner le repository
   ```bash
   git clone [url-du-repo]
   ```

2. Ouvrir la solution dans Visual Studio
   - Double-cliquer sur `PasswordManager.sln`
   - Ou via Visual Studio : File > Open > Project/Solution

3. Configurer le projet
   - Clic droit sur la Solution dans l'explorateur de solution
   - Sélectionner "Set Startup Projects..."
   - Choisir "Multiple startup projects"
   - Mettre "Start" pour :
     - PasswordManager.Api
     - PasswordManager.Web

## Fonctionnalités principales

- [x] Authentification (3pts) : Inscription, connexion.
- [x] Gestion des mots de passe (2pts) : Ajouter, modifier et supprimer des mots de passe.
- [x] Catégorisation (2pts) : Organiser les mots de passe par catégorie.
- [x] Recherche rapide (2pts) : Trouver facilement des mots de passe via une barre de recherche.
- [x] Chiffrement (3pts) : Sécuriser les données avec des algorithmes de chiffrement robustes.
- [ ] Mode hors ligne (2pts) : Accès local sécurisé sans dépendance réseau.
- [x] Sécurité (3pts) : Utilisation d'un mot de passe principal pour la décryptage, verrouillage après tentatives échouées.
- [x] Générateur de mots de passe (2pts) : Génération de mots de passe complexes et sécurisés avec critères personnalisables.
- [x] Sauvegarde (5pts) : sur une base de données SQLite.
- [x] Interface utilisateur (4pts) : Simple, intuitive, et responsive avec recherche et filtrage des mots de passe.
- [ ] Points bonus (5pts): Tests unitaires, et Application mobile

## Technologies utilisées

- [x] Framework front-end (5pts) : Blazor
- [x] Backend (2pts) : ASP.NET Core Web API
- [x] Base de données : SQLite
- [x] Autres (5pts) : Entity Framework Core, Dependency Injection, etc.

## Auteurs

- Frédéric LAUSSON | M1 - nws
