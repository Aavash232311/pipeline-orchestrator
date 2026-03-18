from fastapi import APIRouter
from app.database import get_pool

router = APIRouter()

@router.get("/")
def read_root():
    return {"message": "TrueHire Pipeline running"}


@router.get("/skills")
async def get_skills():
    pool = await get_pool()
    async with pool.acquire() as conn:
        rows = await conn.fetch("SELECT * FROM PROGRAMMING_LANG LIMIT 5")
        return [dict(row) for row in rows]