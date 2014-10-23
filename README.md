DreamScheduler
==============

SOEN 341 project - DreamTeam 

----------------------------------------------------------
- this is the main repo for the application
- everyone must fork (copy) to his own account
- when you create a new feature -> pull request -> we can then check it and add it to the main one

-----------------------------------------------------------
### Basic app

- install neo4j 
- start the database 
- your database port must match this "http://localhost:7474/db/data" `if not you will need to update the code in the controller`

- The current app is just showing you the basic setup of a mvc application
 - In the controller: 
   - there is one function Database()
    - it will create one user in the database
    - it will retrieve that user and send it to the view
    - other function are just redirecting to other view pages
     
### current app 

 - still has the function from basic app if you want to understand the neo4jclient
 - home page has a basic design 
 - Login link in nav ( right side)
  - if clicked it will redirect to login page
    - user can login
    - if user forget to fill a box, system will display a warning
    - if user user a non existing account, system will display a danger notice
    - if user enter wrong password : system display warning
    - if successful : redirect to about page //for testing only will be changed
    - also if succesful : login tab is changed to your username
    - if user click create account, redirect to create page
      - this page has same validation
      - for now when user is created, redirect to about page
      - `i will need to add protection against creating account with same username`
 - to logout
      - click username tab, then choose logout 
  
### so basic function =  create account, login, logout , validation   
