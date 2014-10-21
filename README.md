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
