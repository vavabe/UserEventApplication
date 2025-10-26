CREATE TABLE user_event_stats (
       user_id INT NOT NULL,
       event_type VARCHAR(50) NOT NULL,
       count INT NOT NULL,
       time_interval TIMESTAMP NOT NULL,
       PRIMARY KEY (user_id, event_type, time_interval)
     );