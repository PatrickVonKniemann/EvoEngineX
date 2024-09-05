// Switch to the evoenginex_db database (it will be created if it doesn't exist)
db = db.getSiblingDB("evoenginex_db");

// Create a user for evoenginex_db with readWrite permissions
db.createUser({
    user: "kolenpat",
    pwd: "sa",
    roles: [
        { role: "readWrite", db: "evoenginex_db" }, // Assign readWrite role on evoenginex_db
        { role: "readWrite", db: "admin" }          // Optionally, assign a role on the admin database as well
    ]
});

// Optionally, you can create a collection to ensure the database is fully initialized
db.createCollection("initial_collection");
