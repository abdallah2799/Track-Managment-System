import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import { Artist, CreateArtistRequest } from '../models/artist.model';

@Injectable({ providedIn: 'root' })
export class ArtistService {
  private readonly http = inject(HttpClient);

  getAll(): Observable<Artist[]> {
    return this.http.get<Artist[]>(`${environment.apiUrl}/artists`);
  }

  create(request: CreateArtistRequest): Observable<Artist> {
    return this.http.post<Artist>(`${environment.apiUrl}/artists`, request);
  }
}
