import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';

import { TrackService } from '../../core/services/track.service';
import { Track, TrackStatus } from '../../core/models/track.model';

const STATUS_OPTIONS: TrackStatus[] = ['Draft', 'Submitted', 'Distributed'];

@Component({
  selector: 'app-track-list',
  imports: [FormsModule, RouterLink],
  templateUrl: './track-list.html',
  styleUrl: './track-list.css',
})
export class TrackList {
  private readonly trackService = inject(TrackService);

  readonly statusOptions = STATUS_OPTIONS;
  readonly statusFilter = signal<TrackStatus | ''>('');
  readonly tracks = signal<Track[]>([]);
  readonly loading = signal(false);
  readonly error = signal<string | null>(null);

  constructor() {
    this.loadTracks();
  }

  onStatusFilterChange(): void {
    this.loadTracks();
  }

  private loadTracks(): void {
    this.loading.set(true);
    this.error.set(null);

    const status = this.statusFilter();
    this.trackService.getFiltered(status ? { status } : {}).subscribe({
      next: (tracks) => {
        this.tracks.set(tracks);
        this.loading.set(false);
      },
      error: () => {
        this.error.set('Failed to load tracks. Is the API running?');
        this.loading.set(false);
      },
    });
  }
}
