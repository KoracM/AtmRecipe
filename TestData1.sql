USE AtmRecipeDB;
INSERT INTO Products
    (Name)
VALUES
    ('ATM Basic'),
    ('ATM Pro');

INSERT INTO Components
    (Name, Price)
VALUES
    ('LCD Screen', 500),
    ('Touch Screen', 800),
    ('Intel CPU', 1200),
    ('AMD CPU', 1000);

INSERT INTO ProductComponents
    (ProductId, ComponentId, Quantity)
VALUES
    (1, 1, 1),
    (1, 3, 1),
    (2, 2, 1),
    (2, 4, 1);