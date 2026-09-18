import { Routes } from '@angular/router';

import { TrackList } from './features/track-list/track-list';
import { TrackDetail } from './features/track-detail/track-detail';

export const routes: Routes = [
  { path: '', component: TrackList },
  { path: 'tracks/:id', component: TrackDetail },
];
