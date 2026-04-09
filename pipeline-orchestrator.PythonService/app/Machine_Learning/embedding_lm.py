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
    
