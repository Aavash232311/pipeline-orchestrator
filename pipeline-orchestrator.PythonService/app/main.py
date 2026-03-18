import os
import sys
import uvicorn
from fastapi import FastAPI

# add the project root to Python path,
# Things are little complicated here, we have dotnet microservice and python microservice running this thing
sys.path.insert(0, os.path.dirname(os.path.dirname(os.path.abspath(__file__))))
from app.routes.view import router

app = FastAPI(title="TrueHire Pipeline")

app.include_router(router)

if __name__ == "__main__":
    # Aspire sends the port via the 'PORT' environment variable
    port = int(os.getenv("PORT", 8000))
    uvicorn.run("app.main:app", host="0.0.0.0", port=port)