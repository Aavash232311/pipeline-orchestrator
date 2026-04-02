import os
import csv
import uuid
from nbconvert import export
import psycopg2
import pandas as pd
from dotenv import load_dotenv
from collections import Counter

load_dotenv()
''' It's about time and why is it making me do datascience work I really don't know,
maybe when connecting the dots in future it will make sense but I was supposed to be software engineer. '''
def pipe_to_postgres_array(text):
    if pd.isna(text) or not text:
        return "{}"  
    
    items = text.split('|')
    formatted_items = ",".join(f'"{item}"' for item in items)
    return f"{{{formatted_items}}}"

df = pd.read_csv("../Data/set/postings_sample.csv")


'''
We have normalized table so let's break down the data into subframe.

'''
df.drop('Case', axis=1, inplace=True)
df['WeightsId'] = [str(uuid.uuid4()) for _ in range(len(df))] # first create this unique GUID mapping and then separate the table
df['ListingId'] = [str(uuid.uuid4()) for _ in range(len(df))]
df_cols = df.columns.tolist()

weight_cols = ['Weight_SkillMatch', 'Weight_SkillDepth', 'Weight_Experience', 'Weight_Education', 'Weight_Certifications', 'Weight_ActiveContribs']
listing_table = ['ListingCountry', 'ListingCity', 'ListingAddress', 'ListingLat', 'ListingLong', 'IsRemote', 'RelocateAvailable']
# In our posting we need to eliminate this one column called case
posting_cols = [i for i in df_cols if i not in weight_cols and i not in listing_table]
listing_cols = [i for i in df_cols if i in listing_table]
df_listing = df[listing_cols]
df_posting = df[posting_cols]
df_weights = df[weight_cols]

# Let's assign navigational property (forgegin key) to these tables
df_listing['Id'] = df_posting['ListingId']
df_weights['Id'] = df_posting['WeightsId']

''' Fetch the SQL to get order of this csv and autoamte this pipeline. '''
connection_string = os.getenv('CONNECTION_STRING')
conn = psycopg2.connect(connection_string)
cur = conn.cursor()


def get_sql_columns(table_name):
    cur.execute(
        """
        SELECT ARRAY_AGG(column_name ORDER BY ordinal_position) 
            FROM information_schema.columns 
        WHERE table_name = %s;
        """, (table_name,)
    )
    ''' By default postgres expects lower case form us '''
    return cur.fetchall()[0][0].strip("{}").split(",")

def quoute_sql_columns(columns):
    return [f'"{col}"' for col in columns]

''' Now let's make sure our sql header matches with these '''

posting_sql_columns = get_sql_columns('posting')
listing_sql_columns = get_sql_columns('listing')
weights_sql_columns = get_sql_columns('ScoringWeights')


''' First let's parse to string such that it get's converted into sql array '''

df_posting['PreferredSkills'] = df_posting['PreferredSkills'].apply(pipe_to_postgres_array)
df_posting['RequiredSkills'] = df_posting['RequiredSkills'].apply(pipe_to_postgres_array)
df_posting['RequiredLanguages'] = df_posting['RequiredLanguages'].apply(pipe_to_postgres_array)

df_posting = df_posting[posting_sql_columns]
df_posting.columns = df_posting.columns.str.lower() # again postgres expects lower case

df_weights = df_weights[weights_sql_columns]
df_weights.columns = df_weights.columns.str.lower()


'''
.NET ORM mapped the JobType to enum, so it's basically
integer we need to make sure we do data cleaning here.
public enum JobType
{
    FullTime,
    PartTime,
    Contract,
    Internship,
    Freelance
}

 '''
job_type_mapping = {
    'FullTime': 0,
    'PartTime': 1,
    'Contract': 2,
    'Internship': 3,
    'Freelance': 4
}

df_posting['jobtype'] = df_posting['jobtype'].map(job_type_mapping)

# export this out now
posting_export_path = "./Export/posting.csv" 
weights_export_path = "./Export/weights.csv"
listing_export_path = "./Export/listing.csv"
if not os.path.isfile(posting_export_path) and not os.path.isfile(weights_export_path) and not os.path.isfile(listing_export_path): # if already then do not override this
    df_posting.to_csv(posting_export_path, index=False)
    df_weights.to_csv(weights_export_path, index=False)
    df_listing.to_csv(listing_export_path, index=False)

# there we go this is a real pain for first time to figure out
posting_sql_columns = quoute_sql_columns(posting_sql_columns)
wiehgts_sql_columns = quoute_sql_columns(weights_sql_columns)
listing_sql_columns = quoute_sql_columns(listing_sql_columns)


''' Rule of thumb, table being referenced must exist first so '''

load_posting_sql = f""" 
    COPY posting ({', '.join(posting_sql_columns)})
         FROM STDIN
    WITH (FORMAT CSV, HEADER TRUE);
"""

load_wieght_Sql = f"""
    COPY "ScoringWeights" ({', '.join(wiehgts_sql_columns)})
            FROM STDIN
    WITH (FORMAT CSV, HEADER TRUE);
"""
print(df_weights.columns.tolist())
print(load_wieght_Sql)

load_listing_sql = f"""
    COPY listing ({', '.join(listing_sql_columns)}) 
        FROM STDIN
    WITH (FORMAT CSV, HEADER TRUE);
"""

with open(weights_export_path, 'r', encoding='utf-8') as f:
    cur.copy_expert(sql=load_wieght_Sql, file=f)


conn.commit()
# with open(weights_export_path, 'r', encoding='utf-8') as f:
#     cur.copy_expert(sql=load_wieght_Sql, file=f)


