import os
import uuid
import psycopg2
import pandas as pd
from dotenv import load_dotenv

load_dotenv()
''' 
What is our goal here?
We want to extract the relevant information from the raw data and transform it into a format that can be used for analysis.

We will export that data into a csv file and then load that into a postgresSQL database for further analysis.

Why do we need it?
We want to do analysis on the users resume, in order to do that we need to classify the skills that the users have, and we need to do that in a way that is consistent with the way we classify the skills. 

'''

df = pd.read_csv("../Data/set/digitalSkillsCollection_en.csv")

sub_frame = {
    "id": [str(uuid.uuid4()) for _ in range(len(df))],
    "preferredLabel": df["preferredLabel"],
    "altLabels": df["altLabels"],
    "broaderConceptPT": df["broaderConceptPT"].str.split("|")
}


skill_df = pd.DataFrame(sub_frame)

connection_string = os.getenv('CONNECTION_STRING')

conn = psycopg2.connect(connection_string)
conn.cursor()
