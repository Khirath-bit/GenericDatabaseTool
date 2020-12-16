SELECT
    *
FROM
    TABLENAME t1
INNER JOIN
    TABLENAME t2
ON
    t1.attr = t2.attr
WHERE
    t1.attr = 2;