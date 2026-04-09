from app.database import get_pool
from pydantic import BaseModel
from fastapi import APIRouter
from app.Machine_Learning.embedding_lm import EmbeddingLLM
from uuid import UUID
from typing import Any
from fastapi import FastAPI, HTTPException

class Skill(BaseModel):
    name: str
    difficulty_normalized: float  
    category: str

    class Config:
        from_attributes = True 

router = APIRouter()

@router.get("/")
def read_root():
    return {"message": "Pipeline running"}


@router.get("/skills")
async def get_skills():
    pool = await get_pool()
    async with pool.acquire() as conn:
        rows = await conn.fetch("SELECT * FROM PROGRAMMING_LANG LIMIT 5")
        return [dict(row) for row in rows]

class EmbeddingRequest(BaseModel):
    exp: Any
    summary: Any
    skills: Any
    projects: Any
    posting_id: UUID

''' Side note: my other github repo has explanation about embeddings in depth.'''

# Loaded once the microservice starts, but trade off again 
embedding_model = EmbeddingLLM('sentence-transformers/all-MiniLM-L6-v2')


@router.post("/feature_embeddings")
async def upload_resume_skills(request: EmbeddingRequest):
    learned_embeddings = embedding_model.tokenize([
        request.exp,
        request.summary,
        request.skills,
        request.projects,
    ]).tolist()

    # let's fetch the posting for this job from the datbase and fine the cosine similarity
    '''
    Before using a vector database to store embeddings, let's first check the cosine similarity match
    between the two chunks of text. 
      
     '''
    
    pool = await get_pool()
    

    async with pool.acquire() as conn:
        retrive_post = """
            SELECT * FROM public.posting WHERE "Id" = $1
        """
        rows_posting = await conn.fetch(retrive_post, request.posting_id)
        
        if not rows_posting:
            raise HTTPException(status_code=400, detail="Bad Request MS") 
        
        ''' Basically have some typescheck here, like map to a model, and
         retrive all the information and convert to embeddings.
        And then using torch we will compute the cosine similarity.


        Note:- calling microservice or our API per unit is expensive
        we need to run this service for large pool.

        After unit testing of consine similarity is effective in our case 
        we will convert and store these embeddings into a vector database.
           
         '''
        return [dict(i) for i in rows_posting]


    return {
        'received_payload': request.posting_id
    }