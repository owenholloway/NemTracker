CREATE TABLE schema_versions(
    name varchar(128) NOT NULL,
    executed TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
)