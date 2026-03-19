from __future__ import annotations
from pydantic import BaseModel, ConfigDict, Field
from pydantic.alias_generators import to_camel
from typing import Optional
from datetime import datetime
from uuid import UUID, uuid4


class Education(BaseModel):
    model_config = ConfigDict(alias_generator=to_camel, populate_by_name=True)
    id: UUID
    institution: str
    gpa: Optional[float] = None


class Experience(BaseModel):
    model_config = ConfigDict(alias_generator=to_camel, populate_by_name=True)
    id: UUID
    title: str
    role_description: Optional[str] = None
    currently_working: bool = False
    start_date: datetime
    end_date: Optional[datetime] = None
    institution_name: str
    github_url: Optional[str] = None


class Certification(BaseModel):
    model_config = ConfigDict(alias_generator=to_camel, populate_by_name=True)
    id: UUID
    certification_name: str
    issuing_organization: str
    issue_date: datetime
    expiration_date: Optional[datetime] = None
    credential_url: Optional[str] = None


class URL(BaseModel):
    model_config = ConfigDict(alias_generator=to_camel, populate_by_name=True)
    id: UUID
    url_string: str


class Talent(BaseModel):
    model_config = ConfigDict(alias_generator=to_camel, populate_by_name=True)
    id: UUID = Field(default_factory=uuid4)
    name: Optional[str] = None
    email: Optional[str] = None
    phone: Optional[str] = None
    state: Optional[str] = None
    city: Optional[str] = None
    github_url: Optional[str] = None
    professional_summary: Optional[str] = None
    education: Optional[Education] = None
    experience: Optional[Experience] = None
    skills: list[str] = []
    languages: list[str] = []
    additional_url: Optional[list[URL]] = []
    certifications: Optional[list[Certification]] = []