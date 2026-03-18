import os
import uuid
import psycopg2
import pandas as pd
from dotenv import load_dotenv

''' This script is to create table about,
programming languages we need to classify them at some point '''

load_dotenv()

df = pd.read_csv("../Data/set/technical_skills.csv")
sub_frame = {  # why? because we will use your own id, we might need to scafold in asp.net application
    "id": [str(uuid.uuid4()) for _ in range(len(df))],
    'name': df['Skill Name'],
    'category': df['Category'],
}
sub_frame = pd.DataFrame(sub_frame)

connection_string = os.getenv('CONNECTION_STRING')

conn = psycopg2.connect(connection_string)
cur = conn.cursor()


with open("query/programming_lang.sql", "r") as file:
    create_table = file.read()

''' Execute '''
with conn.cursor() as cursor:
    cursor.execute(create_table)

conn.commit()


''' We need to export that dataframe, cause there we
 have UUID'''

if not os.path.isfile("../Data/subset/subset.csv"): # if already then do not override this
    sub_frame.to_csv("./Export/programming.csv", index=False)

copy_sql = """
    COPY programming_lang 
    FROM STDIN 
    WITH (FORMAT CSV, HEADER)
"""

# Load the data in table, small one but let's load in SQL
with open('./Export/programming.csv', 'r', encoding='utf-8') as f:
    cur.copy_expert(sql=copy_sql, file=f)

conn.commit()