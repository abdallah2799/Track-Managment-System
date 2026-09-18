import { Component, effect, inject, signal } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { DatePipe } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';

import { TrackService } from '../../core/services/track.service';
import { TrackDetail as TrackDetailDto } from '../../core/models/track.model';

@Component({
  selector: 'app-track-detail',
  imports: [RouterLink, DatePipe],
  templateUrl: './track-detail.html',
  styleUrl: './track-detail.css',
})
export class TrackDetail {
  private readonly route = inject(ActivatedRoute);
  private readonly trackService = inject(TrackService);

  private readonly paramMap = toSignal(this.route.paramMap);

  readonly track = signal<TrackDetailDto | null>(null);
  readonly loading = signal(false);
  readonly notFound = signal(false);
  readonly error = signal<string | null>(null);

  constructor() {
    // The router reuses this component instance when navigating between
    // /tracks/:id routes with different ids, so the track must be reloaded
    // whenever the id param changes rather than only once in the constructor.
    effect(() => {
      const id = this.paramMap()?.get('id');
      if (id) {
        this.loadTrack(id);
      }
    });
  }

  private loadTrack(id: string): void {
    this.loading.set(true);
    this.notFound.set(false);
    this.error.set(null);
    this.track.set(null);

    this.trackService.getById(id).subscribe({
      next: (track) => {
        this.track.set(track);
        this.loading.set(false);
      },
      error: (err) => {
        this.loading.set(false);
        if (err.status === 404) {
          this.notFound.set(true);
        } else {
          this.error.set('Failed to load track. Is the API running?');
        }
      },
    });
  }
}
