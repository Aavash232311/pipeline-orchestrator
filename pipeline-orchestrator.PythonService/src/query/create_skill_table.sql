-- This is one of my project for solving, some problems but aslo 
-- I am trying to relate with CSC330 class so I am writing queries here

CREATE TABLE IF NOT EXISTS SKILL_TABLE (
    id UUID PRIMARY KEY,
    preferredLabel VARCHAR(255) NOT NULL,
    altLabels VARCHAR(255) NOT NULL,
    broaderConceptPT VARCHAR(255) NOT NULL
)