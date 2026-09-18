export type TrackStatus = 'Draft' | 'Submitted' | 'Distributed';
export type DistributionStatus = 'Pending' | 'Live' | 'Rejected';

export interface Track {
  id: string;
  title: string;
  artistId: string;
  artistName: string;
  isrc: string;
  releaseDate: string;
  genre: string;
  status: TrackStatus;
}

export interface TrackDistribution {
  id: string;
  dspId: string;
  dspName: string;
  submittedAt: string;
  status: DistributionStatus;
}

export interface TrackDetail extends Track {
  distributions: TrackDistribution[];
}

export interface TrackFilter {
  artistId?: string;
  genre?: string;
  status?: TrackStatus;
}

export interface CreateTrackRequest {
  title: string;
  artistId: string;
  isrc: string;
  releaseDate: string;
  genre: string;
}

export interface DistributeTrackRequest {
  dspIds: string[];
}

export interface UpdateTrackStatusRequest {
  status: TrackStatus;
}
