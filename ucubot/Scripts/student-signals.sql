USE ucubot;

CREATE VIEW student_signals AS
    SELECT student.first_name, student.last_name,
        (CASE lesson_signal.SignalType WHEN -1 THEN "Simple" WHEN 0 THEN "Normal" WHEN 1 THEN "Hard" END) AS signal_type,
            COUNT(*) AS count
    FROM lesson_signal
    INNER JOIN student ON student.id = lesson_signal.student_id
    GROUP BY signal_type, student_id;

