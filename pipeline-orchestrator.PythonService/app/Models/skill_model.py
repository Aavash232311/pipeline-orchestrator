from __future__ import annotations
from pydantic import BaseModel, ConfigDict, Field
from pydantic.alias_generators import to_camel
from uuid import UUID


class Skill(BaseModel):
    model_config = ConfigDict(alias_generator=to_camel, populate_by_name=True)
    id: UUID
    name: str
    difficulty_normalized: float  
    category: str