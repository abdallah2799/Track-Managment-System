export interface Artist {
  id: string;
  name: string;
  email: string;
  country: string;
}

export interface CreateArtistRequest {
  name: string;
  email: string;
  country: string;
}
