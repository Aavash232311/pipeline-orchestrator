import torch
from fastapi import APIRouter
from pydantic import BaseModel
from app.database import get_pool
from app.Machine_Learning.embedding_lm import EmbeddingLLM
from app.Machine_Learning.embedding_lm import CosineSimilarity

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

class FeatureRequest(BaseModel):
    candidate: str
    posting: str
    
embedding_model = EmbeddingLLM('BAAI/bge-small-en-v1.5')


@router.post("/feature_embeddings")
async def upload_resume_skills(data: FeatureRequest):
    candidate_token = embedding_model.tokenize(data.candidate).tolist()
    posting_token = embedding_model.tokenize(data.posting).tolist()


    cosine_similarity = CosineSimilarity()
    similarity_score = cosine_similarity.compute_similarity(torch.tensor(candidate_token), torch.tensor(posting_token))

    return {
        'cosineOut': similarity_score
    }