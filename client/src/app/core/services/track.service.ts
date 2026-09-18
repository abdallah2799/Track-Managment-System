import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import {
  CreateTrackRequest,
  DistributeTrackRequest,
  Track,
  TrackDetail,
  TrackFilter,
  UpdateTrackStatusRequest,
} from '../models/track.model';

@Injectable({ providedIn: 'root' })
export class TrackService {
  private readonly http = inject(HttpClient);

  getFiltered(filter: TrackFilter): Observable<Track[]> {
    let params = new HttpParams();
    if (filter.artistId) params = params.set('artistId', filter.artistId);
    if (filter.genre) params = params.set('genre', filter.genre);
    if (filter.status) params = params.set('status', filter.status);

    return this.http.get<Track[]>(`${environment.apiUrl}/tracks`, { params });
  }

  getById(id: string): Observable<TrackDetail> {
    return this.http.get<TrackDetail>(`${environment.apiUrl}/tracks/${id}`);
  }

  create(request: CreateTrackRequest): Observable<Track> {
    return this.http.post<Track>(`${environment.apiUrl}/tracks`, request);
  }

  distribute(id: string, request: DistributeTrackRequest): Observable<TrackDetail> {
    return this.http.post<TrackDetail>(`${environment.apiUrl}/tracks/${id}/distribute`, request);
  }

  updateStatus(id: string, request: UpdateTrackStatusRequest): Observable<Track> {
    return this.http.patch<Track>(`${environment.apiUrl}/tracks/${id}/status`, request);
  }
}
