SELECT --Inner join example. 
    *
FROM
    TABLENAME t1
INNER JOIN
    TABLENAME t2
ON
    t1.attr = t2.attr
WHERE
    t1.attr = 2;

------------------------------------------------

SELECT --Left join example.
    *
FROM
    TABLENAME t1
LEFT JOIN
    TABLENAME t2
ON
    t1.attr = t2.attr
WHERE
    t1.attr = 2;