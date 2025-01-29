# Password Manager Project

## Auteurs
- Fr�d�ric LAUSSON

## Fonctionnalit�s r�alis�es

   Authentification : Inscription, connexion.
   Gestion des mots de passe : Ajouter, modifier et supprimer des mots de passe.
   Cat�gorisation : Organiser les mots de passe par cat�gorie.
   Recherche rapide : Trouver facilement des mots de passe via une barre de recherche.
   Chiffrement : S�curiser les donn�es avec des algorithmes de chiffrement robustes.
   Mode hors ligne : Acc�s local s�curis� sans d�pendance r�seau.
   S�curit� : Utilisation d�un mot de passe principal pour la d�cryptage, verrouillage apr�s tentatives �chou�es.
   G�n�rateur de mots de passe : G�n�ration de mots de passe complexes et s�curis�s avec crit�res personnalisables.
   Sauvegarde : sur une base de donn�es SQLite.
   Interface utilisateur : Simple, intuitive, et responsive avec recherche et filtrage des mots de passe.
   Points bonus: Tests unitaires, et Application mobile

   # Password Manager API

Une API sécurisée pour la gestion de mots de passe, développée avec ASP.NET Core.

## 🚀 Fonctionnalités prévues

### 1. Configuration et Sécurité
- [ ] Classe AppSettings pour centraliser la configuration
- [ ] Validation du JWT token
- [ ] Hachage des mots de passe (bcrypt/argon2)
- [ ] Rate limiting pour prévenir les attaques
- [ ] Logs structurés (Serilog)

### 2. Controllers et Endpoints
#### AuthController
- [ ] Login endpoint
- [ ] Register endpoint
- [ ] Refresh token endpoint
- [ ] Change password endpoint

#### PasswordController
- [ ] CRUD operations
- [ ] Partage de mots de passe
- [ ] Recherche/filtrage
- [ ] Export sécurisé

#### UserController
- [ ] Gestion du profil utilisateur
- [ ] Préférences utilisateur

### 3. Services
#### AuthService
- [ ] Génération de JWT tokens
- [ ] Validation des credentials
- [ ] Gestion des refresh tokens

#### PasswordService
- [ ] Logique de chiffrement/déchiffrement
- [ ] Validation des mots de passe
- [ ] Génération de mots de passe sécurisés

#### UserService
- [ ] Gestion des utilisateurs
- [ ] Validation des données

### 4. Models
#### DTOs
- [ ] LoginDto
- [ ] RegisterDto
- [ ] PasswordDto
- [ ] UserDto
- [ ] TokenDto

#### Entities
- [ ] User
- [ ] Password
- [ ] RefreshToken
- [ ] AuditLog

### 5. Base de données
- [ ] Configuration Entity Framework Core
- [ ] Migrations
- [ ] Repositories
- [ ] Relations entre entités
- [ ] Index pour les performances

### 6. Validation et Erreurs
- [ ] FluentValidation
- [ ] Middleware de gestion d'erreurs global
- [ ] Codes d'erreur personnalisés
- [ ] Validation des modèles

### 7. Tests
#### Tests unitaires
- [ ] Services
- [ ] Controllers
- [ ] Validation

#### Tests d'intégration
- [ ] Flow d'authentification
- [ ] CRUD des mots de passe
- [ ] Scénarios de partage

### 8. Documentation
- [ ] Swagger avec exemples
- [ ] Documentation des endpoints
- [ ] Description des DTOs
- [ ] Documentation des codes
