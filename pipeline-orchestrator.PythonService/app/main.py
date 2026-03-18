import os
from fastapi import FastAPI
from app.routes.view import router

app = FastAPI(title="TrueHire Pipeline")

app.include_router(router)