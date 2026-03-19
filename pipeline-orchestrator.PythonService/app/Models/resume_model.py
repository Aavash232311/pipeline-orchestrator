from __future__ import annotations
from pydantic import BaseModel
from typing import Optional
from datetime import datetime
from uuid import UUID, uuid4

from pydantic import BaseModel, ConfigDict
from pydantic.alias_generators import to_camel

class Education(BaseModel):
    # We need to understand the way c# sends data
    model_config = ConfigDict(alias_generator=to_camel, populate_by_name=True)
    Id: UUID
    Institution: str
    GPA: Optional[float] = None

class Experience(BaseModel):
    model_config = ConfigDict(alias_generator=to_camel, populate_by_name=True)
    Id: UUID
    Title: str
    RoleDescription: Optional[str] = None
    currentlyWorking: bool = False
    StartDate: datetime
    EndDate: Optional[datetime] = None
    InstitutionName: str
    GitHubUrl: Optional[str] = None

class Certification(BaseModel):
    model_config = ConfigDict(alias_generator=to_camel, populate_by_name=True)
    Id: UUID
    CertificationName: str
    IssuingOrganization: str
    IssueDate: datetime
    ExpirationDate: Optional[datetime] = None
    CredentialURL: Optional[str] = None

class URL(BaseModel):
    model_config = ConfigDict(alias_generator=to_camel, populate_by_name=True)
    Id: UUID
    URLString: str

class Talent(BaseModel):
    model_config = ConfigDict(alias_generator=to_camel, populate_by_name=True)
    Id: UUID = uuid4()
    Name: Optional[str] 
    Email: Optional[str]
    Phone: Optional[str] 
    State: Optional[str] 
    City: Optional[str] 
    GitHubUrl: Optional[str] 
    ProfessionalSummary: Optional[str] = None
    Education: Optional[Education] = None
    Experience: Optional[Experience] = None
    Skills: list[str] = []
    Languages: list[str] = []
    AdditionalURL: Optional[list[URL]] = []
    Certifications: Optional[list[Certification]] = []