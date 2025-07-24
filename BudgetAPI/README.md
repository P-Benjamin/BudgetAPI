# BudgetAPI

BudgetAPI est une API RESTful d�velopp�e en ASP.NET Core, qui permet de g�rer des revenus (`Incomes`) et des d�penses (`Outcomes`) avec une authentification s�curis�e via JWT.  
Une interface HTML/JavaScript est �galement fournie pour consommer facilement l�API.

---

## Fonctionnalit�s

- Authentification JWT s�curis�e
- Gestion des utilisateurs avec DTO 
- Cr�ation, modification, suppression de revenus et d�penses
- Calculs de totaux : global, par mois, par ann�e, par p�riode
- Interface web simple (HTML/JS) avec Chart.js
- Swagger int�gr� pour explorer l�API
- Validation de tous les mod�les avec `[Required]`, `[Range]`, etc.
- Base de donn�es en m�moire (EF Core InMemory)

---

## Technologies utilis�es

- ASP.NET Core Web API
- Entity Framework Core (InMemory)
- JWT (Json Web Token)
- Swagger / Swashbuckle
- Chart.js
- HTML, JavaScript 

---

## Installation et ex�cution

1. Cloner le projet :

```bash
git clone https://github.com/P-Benjamin/BudgetAPI.git
cd BudgetAPI
```

2. Acc�der � l�application :

Interface Swagger : https://localhost:7058/swagger

Interface HTML : https://localhost:7058/index.html

## API Endpoints

### Authentification

`POST /api/Login`  
Authentifie un utilisateur et g�n�re un jeton JWT.

---

### Incomes

`GET /api/Incomes`  
R�cup�re la liste de tous les revenus.

`POST /api/Incomes`  
Cr�e un nouveau revenu.

`GET /api/Incomes/{id}`  
R�cup�re un revenu sp�cifique selon son ID.

`PUT /api/Incomes/{id}`  
Met � jour un revenu existant.

`DELETE /api/Incomes/{id}`  
Supprime un revenu existant.

`GET /api/Incomes/total`  
Calcule le revenu total.

`GET /api/Incomes/total/month/{year}/{month}`  
Calcule le revenu total pour un mois et une ann�e donn�s.

`GET /api/Incomes/total/year/{year}`  
Calcule le revenu total pour une ann�e donn�e.

`POST /api/Incomes/total/range`  
Calcule le revenu total sur une plage de dates donn�e.

---

### Outcomes 

`GET /api/Outcomes`  
R�cup�re la liste de toutes les d�penses.

`POST /api/Outcomes`  
Cr�e une nouvelle d�pense.

`GET /api/Outcomes/{id}`  
R�cup�re une d�pense sp�cifique par ID.

`PUT /api/Outcomes/{id}`  
Modifie une d�pense existante.

`DELETE /api/Outcomes/{id}`  
Supprime une d�pense existante.

`GET /api/Outcomes/total`  
Calcule le total de toutes les d�penses.

`GET /api/Outcomes/total/month/{year}/{month}`  
Calcule les d�penses totales pour un mois et une ann�e donn�s.

`GET /api/Outcomes/total/year/{year}`  
Calcule les d�penses totales pour une ann�e donn�e.

`POST /api/Outcomes/total/range`  
Calcule les d�penses totales sur une plage de dates.

---

### Users 

`GET /api/Users`  
R�cup�re la liste de tous les utilisateurs.

`POST /api/Users`  
Cr�e un nouvel utilisateur.

`GET /api/Users/{id}`  
R�cup�re un utilisateur sp�cifique par ID.

`PUT /api/Users/{id}`  
Met � jour un utilisateur existant.

`DELETE /api/Users/{id}`  
Supprime un utilisateur existant par ID.

## Auteurs
PINOSA Benjamin et SUSINI M�gane