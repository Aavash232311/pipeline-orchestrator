from fastapi import APIRouter, HTTPException, Request
from app.Models.resume_model import Talent
from app.database import get_pool
from app.Models.skill_model import Skill
from pydantic import BaseModel

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

''' Run simplest form of ML here, it would be a joke to call this
microservice just to query SQL '''
@router.post("/resume")
async def upload_resume(request: Request):
    data = await request.json()
    talent = Talent(**data)  
    # Array of skills from the resume, now let's classify that using our dataset
    pool = await get_pool()
    async with pool.acquire() as conn:
        result = [f"%{skill}%" for skill in talent.skills]  

        # ex: ['%C#%', '%.NET%', '%React%', '%TypeScript%', '%PostgreSQL%', '%Docker%', '%Kubernetes%']
        skill_fetch_query = f"""
            SELECT * FROM
              PROGRAMMING_LANG
            WHERE name ILIKE ANY($1)
        """
        ''' In c# I am confident about ORM fetching .toListAsync() 
        after that I do filteting in certian case if necessary but in this
        case the language is slow and scaling might be difficult. '''



         # $1 gold standard for preventing SQL
        rows_skills = await conn.fetch(skill_fetch_query, result)

        if not rows_skills: # this language is super slow, and let's do search in SQL level instead.
             raise HTTPException(status_code=404, detail="Item not found")
        # we don't have typecheck in the bloodline here, which is a bummer.
        dict_skills = [dict(row) for row in rows_skills] # this is always small, even if 100 skills


        unique_types = set(row["type"] for row in dict_skills) # these are different types of skills that a user might have.
        unique_categories = set(row["category"] for row in dict_skills) # these are different categories of skills that a user might have.
        unique_fields = set(row["field"] for row in dict_skills) # these are different fields of skills that a user might have.
        return  {
            "all_skills": dict_skills,
            "unique_types": list(unique_types),
            "unique_categories": list(unique_categories),
            "unique_fields": list(unique_fields)
        }
    
    raise HTTPException(status_code=404, detail="Item not found")