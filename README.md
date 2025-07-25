# BudgetAPI

BudgetAPI est une API RESTful développée en ASP.NET Core, qui permet de gérer des revenus (`Incomes`) et des dépenses (`Outcomes`) avec une authentification sécurisée via JWT.  
Une interface HTML/JavaScript est également fournie pour consommer facilement l’API.

---

## Fonctionnalités

- Authentification JWT sécurisée
- Gestion des utilisateurs avec DTO 
- Création, modification, suppression de revenus et dépenses
- Calculs de totaux : global, par mois, par année, par période
- Interface web simple (HTML/JS)
- Swagger intégré pour explorer l’API
- Validation de tous les modèles avec `[Required]`, `[Range]`, etc.
- Base de données en mémoire (EF Core InMemory)

---

## Technologies utilisées

- ASP.NET Core Web API
- Entity Framework Core (InMemory)
- JWT (Json Web Token)
- Swagger / Swashbuckle
- HTML, JavaScript 

---

## Installation et exécution

1. Cloner le projet :

```bash
git clone https://github.com/P-Benjamin/BudgetAPI.git
cd BudgetAPI
```

2. Accéder à l’application :

Interface Swagger : https://localhost:7058/swagger

Interface HTML : https://localhost:7058/index.html

Identifiants : 
 - Username : admin
 - Password : admin123

## API Endpoints

### Authentification

`POST /api/Login`  
Authentifie un utilisateur et génère un jeton JWT.

---

### Sources

`GET /api/sources`
Liste toutes les sources disponibles.

`GET /api/sources/{id}` 
Récupère une source spécifique.

`POST /api/sources`
Crée une nouvelle source.

`PUT /api/sources/{id}`
Met à jour une source existante.

`DELETE /api/sources/{id}`
Supprime une source.

---

### Incomes

`GET /api/Incomes`  
Récupère la liste de tous les revenus.

`POST /api/Incomes`  
Crée un nouveau revenu.

`GET /api/Incomes/{id}`  
Récupère un revenu spécifique selon son ID.

`PUT /api/Incomes/{id}`  
Met à jour un revenu existant.

`DELETE /api/Incomes/{id}`  
Supprime un revenu existant.

`GET /api/Incomes/total`  
Calcule le revenu total.

`GET /api/Incomes/total/month/{year}/{month}`  
Calcule le revenu total pour un mois et une année donnés.

`GET /api/Incomes/total/year/{year}`  
Calcule le revenu total pour une année donnée.

`POST /api/Incomes/total/range`  
Calcule le revenu total sur une plage de dates donnée.

`GET /api/incomes/by-source/{sourceId}`
Liste tous les revenus d’une source spécifique.

---

### Outcomes 

`GET /api/Outcomes`  
Récupère la liste de toutes les dépenses.

`POST /api/Outcomes`  
Crée une nouvelle dépense.

`GET /api/Outcomes/{id}`  
Récupère une dépense spécifique par ID.

`PUT /api/Outcomes/{id}`  
Modifie une dépense existante.

`DELETE /api/Outcomes/{id}`  
Supprime une dépense existante.

`GET /api/Outcomes/total`  
Calcule le total de toutes les dépenses.

`GET /api/Outcomes/total/month/{year}/{month}`  
Calcule les dépenses totales pour un mois et une année donnés.

`GET /api/Outcomes/total/year/{year}`  
Calcule les dépenses totales pour une année donnée.

`POST /api/Outcomes/total/range`  
Calcule les dépenses totales sur une plage de dates.

`GET /api/outcomes/by-source/{sourceId}`
Liste toutes les dépenses d’une source spécifique.


---

### Users 

`GET /api/Users`  
Récupère la liste de tous les utilisateurs.

`POST /api/Users`  
Crée un nouvel utilisateur.

`GET /api/Users/{id}`  
Récupère un utilisateur spécifique par ID.

`PUT /api/Users/{id}`  
Met à jour un utilisateur existant.

`DELETE /api/Users/{id}`  
Supprime un utilisateur existant par ID.

---

## Auteurs
PINOSA Benjamin et SUSINI Mégane
