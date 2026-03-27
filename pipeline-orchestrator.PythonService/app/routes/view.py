from fastapi import APIRouter, HTTPException, Request
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
    # Array of skills from the resume, now let's classify that using our dataset
    pool = await get_pool()
    async with pool.acquire() as conn:
        result = [f"%{skill}%" for skill in talent.skills]  

        # ex: ['%C#%', '%.NET%', '%React%', '%TypeScript%', '%PostgreSQL%', '%Docker%', '%Kubernetes%']
        query = f"SELECT * FROM PROGRAMMING_LANG WHERE name ILIKE ANY($1)" # $1 gold standard for preventing SQL
        rows = await conn.fetch(query, result)

        if rows:
            skills = [dict(row) for row in rows]
            return skills
    
    raise HTTPException(status_code=404, detail="Item not found")