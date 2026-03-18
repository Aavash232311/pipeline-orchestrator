import os
import asyncpg
from pathlib import Path
from dotenv import load_dotenv

# For data analysis same environmant is used,
# Different convention so that asyncpg accepts it
# Even ASP.NET ORM uses same database, CRAZY
load_dotenv(Path(__file__).parent.parent / "src" / ".env") 

DATABASE_URL = os.getenv("DATABASE_URL")

_pool = None

async def get_pool():
    global _pool
    if _pool is None:
        _pool = await asyncpg.create_pool(DATABASE_URL)
    return _pool

async def get_connection():
    pool = await get_pool()
    return pool.acquire()