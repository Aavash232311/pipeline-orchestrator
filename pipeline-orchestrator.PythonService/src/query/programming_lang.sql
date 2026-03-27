

CREATE TABLE IF NOT EXISTS PROGRAMMING_LANG (
    id UUID PRIMARY KEY,
    name VARCHAR(255) NULL,
    category VARCHAR(255) NULL,
    type VARCHAR(255) NULL,
    field VARCHAR(255) NULL,
    ecosystem VARCHAR(255) NULL,
    difficulty INT NOT NULL DEFAULT 1,
    -- Logistic sigmoid making it center at 5, -x (centered at 5, scaled by 0.8) because for a very small value it might show high
    difficulty_normalized DECIMAL(5,4) GENERATED ALWAYS AS ( -- 5 digit with 4 decimal
        1.0 / (1.0 + POWER(EXP(1), -0.8 * (difficulty - 5)))
    ) STORED
);