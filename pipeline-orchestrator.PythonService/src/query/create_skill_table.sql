-- This is one of my project for solving, some problems but aslo 
-- I am trying to relate with CSC330 class so I am writing queries here

CREATE TABLE IF NOT EXISTS SKILL_TABLE (
    id UUID PRIMARY KEY,
    preferredLabel VARCHAR(2255) NULL,
    altLabels VARCHAR(2255) NULL,
    broaderConceptPT VARCHAR(2255) NULL
)