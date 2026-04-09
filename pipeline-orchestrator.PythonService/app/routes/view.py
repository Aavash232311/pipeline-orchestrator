from fastapi import Request
from app.database import get_pool
from pydantic import BaseModel
from fastapi import APIRouter
from app.Machine_Learning.embedding_lm import EmbeddingLLM
from typing import Any


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

''' Side note: my other github repo has explanation about embeddings in depth.'''

# Loaded once the microservice starts, but trade off again 
embedding_model = EmbeddingLLM('sentence-transformers/all-MiniLM-L6-v2')


@router.post("/feature_embeddings")
async def upload_resume_skills(request: EmbeddingRequest):
    learned_embeddings = embedding_model.tokenize([
        request.exp,
        request.summary,
        request.skills,
        request.projects
    ]).tolist()
    return {
        'received_payload': learned_embeddings
    }