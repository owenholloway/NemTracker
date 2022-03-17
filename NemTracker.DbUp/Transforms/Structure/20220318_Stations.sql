CREATE TABLE Stations(
    id uuid NOT NULL PRIMARY KEY,
    participant_id UUID,
    station_name varchar(256),
    region smallint NOT NULL DEFAULT -1,
    technology_type smallint  NOT NULL DEFAULT -1,
    technology_type_descriptor smallint  NOT NULL DEFAULT -1,
    physical_unit_min int NOT NULL DEFAULT 0,
    physical_unit_max int NOT NULL DEFAULT 0,
    unit_size_mw  float8,
    duid varchar(32),
    dispatch_type smallint  NOT NULL DEFAULT -1
);

CREATE INDEX IX_Stations_station_name ON Stations (station_name);
CREATE INDEX IX_Stations_duid ON Stations (duid);
CREATE INDEX IX_Stations_participant_id ON Stations (participant_id);
CREATE INDEX IX_Stations_technology_type ON Stations (technology_type);