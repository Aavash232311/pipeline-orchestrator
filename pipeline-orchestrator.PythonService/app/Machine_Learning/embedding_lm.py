import torch.nn.functional as F
from sentence_transformers import SentenceTransformer


''' 
Our goal is to see cosine similarity here, between resume and
all the posting of job. 

'''
class EmbeddingLLM:
    def __init__(self, embedding_model):
        self.model = SentenceTransformer(embedding_model)

    def tokenize(self, text_list):
        return self.model.encode(text_list)
    

class CosineSimilarity:

    def compute_similarity(self, embedding1, embedding2):
        score = F.cosine_similarity(embedding1.unsqueeze(0), embedding2.unsqueeze(0))
        return max(-1.0, min(1.0, score.item()))