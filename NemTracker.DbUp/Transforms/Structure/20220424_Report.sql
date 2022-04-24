CREATE TABLE reports(
     id bigserial PRIMARY KEY,
     publish_date_time timestamp,
     interval_date_time timestamp,
     path text,
     processed boolean,
     interval_process_type smallint,
     report_type smallint
);

CREATE INDEX IX_reports_publish_date_time ON reports (publish_date_time);
CREATE INDEX IX_reports_interval_date_time ON reports (interval_date_time);
CREATE INDEX IX_reports_interval_process_type ON reports (interval_process_type);
CREATE INDEX IX_reports_report_type ON reports (report_type);