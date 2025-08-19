# sav-project.net

**Mini Projet SAV** – Full-stack application pour gérer un Service Après-Vente (SAV).

---

##  Description
Ceci est une application complète développée avec **Blazor**, **.NET**, **ASP.NET Core**, et un backend **API OCR**. Elle comprend :

- **BlazorFrontend** : interface utilisateur moderne et réactive  
- **MiniProjet** : backend .NET principal  
- **OCRAppApi** : microservice pour reconnaissance optique de caractères (OCR)  
- **Shared** : librairie commune partagée entre frontend & backend  

---

##  Fonctionnalités principales
- Tableau de bord SAV avec gestion des réclamations  
- Authentification par rôles (Admin & User)  
- Uploads de documents/scans avec lecture automatique via OCR  
- API bien structurée pour traitement des requêtes  

---

##  Stack technique
- **Frontend** : Blazor WebAssembly / Server  
- **Backend / API** : ASP.NET Core (.NET 8), C#  
- **OCR** : Service OCR personnalisé  
- **Base de données** : MySQL / SQL Server (selon configuration)  

---

##  Installation & exécution

1. Cloner le dépôt :
   ```bash
   git clone https://github.com/ayman-trabelsi/sav-project.net.git
   cd sav-project.net
