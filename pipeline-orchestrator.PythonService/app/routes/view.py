from app.database import get_pool
from pydantic import BaseModel
from fastapi import APIRouter
from app.Machine_Learning.embedding_lm import EmbeddingLLM


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


''' Side note: my other github repo has explanation about embeddings in depth.'''

# Loaded once the microservice starts, but trade off again 
embedding_model = EmbeddingLLM('sentence-transformers/all-MiniLM-L6-v2')

@router.post("/feature_embeddings")
async def upload_resume_skills(features: list[str]):
    return {
        'Embeddings': embedding_model.tokenize(features).tolist()
    }