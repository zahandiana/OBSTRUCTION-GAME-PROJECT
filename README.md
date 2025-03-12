# Obstruction Game 
## 📌 Descriere
Acest proiect este o aplicație completă pentru jocul **Obstruction**, implementată folosind **C#** și **Angular**. Backend-ul gestionează logica jocului printr-un API RESTful, iar frontend-ul oferă o interfață interactivă pentru utilizatori.

Jocul **Obstruction** este un joc strategic, în care fiecare mutare blochează celulele adiacente. Obiectivul este de a forța adversarul să rămână fără mutări valide.

## 🛠️ Tehnologii utilizate
### Backend
- **C#**
- **.NET** (ASP.NET Core, Entity Framework, Identity)
- **SQL** pentru stocarea datelor
- **Docker** pentru containere
- **Git, Bitbucket, Jira** pentru versionare și managementul proiectului
- **Linux, Python, Bash** (opțional, pentru debugging și scripting)

### Frontend
- **HTML, CSS, JavaScript, TypeScript**
- **Angular** pentru interfață
- **HTTP** (protocol de rețea) pentru comunicare cu backend-ul

## 🔧 Funcționalități
- **Algoritm de joc implementat**
  - Mutări strategice bazate pe analiza tablei de joc
  - Comparare cu un algoritm random pentru evaluare
- **API RESTful**
  - Permite trimiterea și procesarea mutărilor
  - Gestionarea sesiunilor de joc
- **Frontend interactiv**
  - Interfață dezvoltată în Angular pentru interacțiunea utilizatorilor
  - Afișarea stării jocului și actualizarea în timp real
- **Sistem modular pentru algoritmi**
  - Suportă implementări multiple ale strategiei de joc
  
## 🚀 Instalare și rulare
### Backend
1. Clonează repository-ul:
   ```sh
   git clone [https://github.com/user/obstruction-game.git](https://github.com/zahandiana/OBSTRUCTION-GAME-PROJECT)
   ```
2. Deschide proiectul în **Visual Studio**.
3. Compilează și rulează aplicația.

### Frontend

1. Instalează dependențele Angular:
   ```sh
   npm install
   ```
3. Rulează serverul de dezvoltare:
   ```sh
   ng serve --open
   ```
4. Accesează aplicația în browser.

## 📂 Structura codului
### Backend
- **Program.cs** – Inițializarea serverului și configurarea middleware-ului.
- **JocController.cs** – Implementarea endpoint-urilor API pentru gestionarea jocului.
- **AlgoritmJocAttribute.cs** – Atribut personalizat pentru identificarea diferitelor strategii de joc.
- **DescriereAlgoritm.cs** – Definirea metadatelor pentru algoritmii disponibili.
- **ReturnIndexByDefault.cs** – Middleware pentru gestionarea redirecționării paginilor UI.

### Frontend
- **index.html** – Punctul de intrare al aplicației.
- **app.component.ts** – Componenta principală.
- **services/game.service.ts** – Comunicare cu backend-ul.
- **components/game-board.component.ts** – Afișarea tablei de joc și interacțiunea utilizatorului.

## 🛠️ Îmbunătățiri viitoare
- Optimizarea algoritmului AI pentru un joc mai competitiv.
- Implementarea unui mod multiplayer cu suport pentru sesiuni online.
- Integrarea cu baze de date NoSQL pentru stocare flexibilă a datelor.
