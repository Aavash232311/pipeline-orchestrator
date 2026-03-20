from fastapi import APIRouter, Request
from app.Models.resume_model import Talent
from app.database import get_pool

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
    
@router.post("/resume")
async def upload_resume(request: Request):
    data = await request.json()
    talent = Talent(**data)  
    skills = talent.skills
    return {
        "pool": skills
    }