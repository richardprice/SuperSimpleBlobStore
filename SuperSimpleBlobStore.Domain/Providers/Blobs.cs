using System;
using SuperSimpleBlobStore.DataAccess;
using SuperSimpleBlobStore.DataAccess.Model;
using SuperSimpleBlobStore.Domain.Exceptions;
using Blob = SuperSimpleBlobStore.Api.ViewModel.Blobs.Blob;

namespace SuperSimpleBlobStore.Domain.Providers
{
    public class Blobs : IBlobs
    {
        private readonly IBlobRepository _blobRepository;
        private readonly IContainerRepository _containerRepository;

        public Blobs(IBlobRepository blobRepository, IContainerRepository containerRepository)
        {
            _blobRepository = blobRepository;
            _containerRepository = containerRepository;
        }

        public Blob GetBlob(Guid id)
        {
            try
            {
                var blob = _blobRepository.GetLatestValidBlob(id);

                if (blob == null)
                    throw new BlobNotFoundException();

                return new Blob
                {
                    BlobId = blob.BlobId,
                    BlobCreatedBy = blob.BlobCreatedBy,
                    BlobCreatedOn = blob.BlobCreatedOn,
                    BlobValidFrom = blob.BlobValidFrom,
                    BlobValidTo = blob.BlobValidTo,
                    ContainerIdentity = blob.ContainerIdentity,
                    BlobLastModified = blob.BlobLastModified,
                    BlobLastModifiedBy = blob.BlobLastModifiedBy,
                    VersionId = blob.VersionId,
                    VersionCreatedOn = blob.VersionCreatedOn,
                    VersionValidFrom = blob.VersionValidFrom,
                    VersionValidTo = blob.VersionValidTo,
                    VersionLastModified = blob.VersionLastModified,
                    VersionLastModifiedBy = blob.VersionLastModifiedBy,
                    BlobContentsId = blob.BlobContentsId,
                    Etag = blob.Etag,
                    ContentLength = blob.ContentLength,
                    ContentType = blob.ContentType,
                    FileName = blob.FileName,
                    BlobVariantHash = blob.BlobVariantHash,
                };
            }
            catch(Exception ex)
            {
                throw new CouldNotGetBlobException("Blobs.GetBlob", ex);
            }
        }

        public Blob GetBlobVersion(Guid id, Guid versionId)
        {
            try
            {
                var blob = _blobRepository.GetSpecificBlobVersion(id, versionId);

                if (blob == null)
                    throw new BlobNotFoundException();

                return new Blob
                {
                    BlobId = blob.BlobId,
                    BlobCreatedBy = blob.BlobCreatedBy,
                    BlobCreatedOn = blob.BlobCreatedOn,
                    BlobValidFrom = blob.BlobValidFrom,
                    BlobValidTo = blob.BlobValidTo,
                    ContainerIdentity = blob.ContainerIdentity,
                    BlobLastModified = blob.BlobLastModified,
                    BlobLastModifiedBy = blob.BlobLastModifiedBy,
                    VersionId = blob.VersionId,
                    VersionCreatedOn = blob.VersionCreatedOn,
                    VersionValidFrom = blob.VersionValidFrom,
                    VersionValidTo = blob.VersionValidTo,
                    VersionLastModified = blob.VersionLastModified,
                    VersionLastModifiedBy = blob.VersionLastModifiedBy,
                    BlobContentsId = blob.BlobContentsId,
                    Etag = blob.Etag,
                    ContentLength = blob.ContentLength,
                    ContentType = blob.ContentType,
                    FileName = blob.FileName,
                    BlobVariantHash = blob.BlobVariantHash,
                };
            }
            catch(Exception ex)
            {
                throw new CouldNotGetBlobException("Blobs.GetBlobVersion", ex);
            }
        }

        public Blob GetBlobForDate(Guid id, DateTime date)
        {
            try
            {
                var blob = _blobRepository.GetBlobForDate(id, date);

                if (blob == null)
                    throw new BlobNotFoundException();

                return new Blob
                {
                    BlobId = blob.BlobId,
                    BlobCreatedBy = blob.BlobCreatedBy,
                    BlobCreatedOn = blob.BlobCreatedOn,
                    BlobValidFrom = blob.BlobValidFrom,
                    BlobValidTo = blob.BlobValidTo,
                    ContainerIdentity = blob.ContainerIdentity,
                    BlobLastModified = blob.BlobLastModified,
                    BlobLastModifiedBy = blob.BlobLastModifiedBy,
                    VersionId = blob.VersionId,
                    VersionCreatedOn = blob.VersionCreatedOn,
                    VersionValidFrom = blob.VersionValidFrom,
                    VersionValidTo = blob.VersionValidTo,
                    VersionLastModified = blob.VersionLastModified,
                    VersionLastModifiedBy = blob.VersionLastModifiedBy,
                    BlobContentsId = blob.BlobContentsId,
                    Etag = blob.Etag,
                    ContentLength = blob.ContentLength,
                    ContentType = blob.ContentType,
                    FileName = blob.FileName,
                    BlobVariantHash = blob.BlobVariantHash,
                };
            }
            catch(Exception ex)
            {
                throw new CouldNotGetBlobException("Blobs.GetBlobForDate", ex);
            }
        }
        public bool DeleteBlob(Guid id)
        {
            try
            {
                var blob = _blobRepository.GetBlobDetails(id);

                if (blob == null)
                    throw new BlobNotFoundException();

                return _blobRepository.DeleteBlobDetails(blob);
            }
            catch(Exception ex)
            {
                throw new CouldNotDeleteBlobException("Blobs.DeleteBlob", ex);
            }
        }

        public byte[] GetContents(Guid id)
        {
            try
            {
                var blob = _blobRepository.GetLatestValidBlobContents(id);

                if (blob == null)
                    throw new BlobNotFoundException();

                return blob;
            }
            catch(Exception ex)
            {
                throw new CouldNotGetBlobException("Blobs.GetContents", ex);
            }
        }

        public byte[] GetContentsForVersion(Guid id, Guid versionId)
        {
            try
            {
                var blob = GetBlobVersion(id, versionId);

                if (blob == null)
                    throw new CouldNotGetBlobException();

                var contents = _blobRepository.GetBlobContents(blob.BlobContentsId);

                if (contents == null)
                    throw new BlobNotFoundException();

                return contents.Content;
            }
            catch(Exception ex)
            {
                throw new CouldNotGetBlobException("Blobs.GetContentsForVersion", ex);
            }
        }

        public Guid CreateBlob(byte[] contents, string fileName, string etag, string mimeType, Guid containerIdentity, Guid creatorId)
        {
            var container = _containerRepository.GetValidContainer(containerIdentity);

            if (container == null)
                throw new CouldNotCreateBlobException();

            int contentsId;

            try
            {
                var blobContents = _blobRepository.CreateBlobContents(new BlobContents {Content = contents});

                if (blobContents == null)
                    throw new CouldNotCreateBlobException();

                contentsId = blobContents.Id;
            }
            catch(Exception ex)
            {
                throw new CouldNotCreateBlobException("Blobs.CreateBlob", ex);
            }

            var blobDetails = new BlobDetails
            {
                BlobId = Guid.NewGuid(),
                CreatedBy = creatorId,
                CreatedOn = DateTime.Now,
                ValidFrom = DateTime.Now,
                ContainerId = container.Id,
                LastModified = DateTime.Now,
                LastModifiedBy = creatorId
            };

            try
            {
                blobDetails = _blobRepository.CreateBlobDetails(blobDetails);

                if (blobDetails == null)
                    throw new CouldNotCreateBlobException();
            }
            catch(Exception ex)
            {
                throw new CouldNotCreateBlobException("Blobs.CreateBlob", ex);
            }

            var blobVersion = new BlobVersion
            {
                VersionId = Guid.NewGuid(),
                CreatedBy = blobDetails.CreatedBy,
                CreatedOn = blobDetails.CreatedOn,
                ValidFrom = blobDetails.ValidFrom,
                LastModifiedBy = blobDetails.LastModifiedBy,
                LastModified = blobDetails.LastModified,
                BlobContentsId = contentsId,
                Etag = etag,
                ContentLength = contents.Length,
                ContentType = mimeType,
                FileName = fileName,
                BlobId = blobDetails.Id,
                BlobVariantHash = string.Empty
            };

            try
            {
                blobVersion = _blobRepository.CreateBlobVersion(blobVersion);

                if (blobVersion == null)
                    throw new CouldNotCreateBlobException();
            }
            catch(Exception ex)
            {
                throw new CouldNotCreateBlobException("Blobs.CreateBlob", ex);
            }

            return blobDetails.BlobId;
        }

        public Guid CreateBlobVersion(Guid blobId, byte[] contents, string fileName, string etag, string mimeType, Guid creatorId)
        {
            var existingBlob = _blobRepository.GetBlobDetails(blobId);

            if (existingBlob == null)
                throw new BlobNotFoundException();

            int contentsId;

            try
            {
                var blobContents = _blobRepository.CreateBlobContents(new BlobContents { Content = contents });

                if (blobContents == null)
                    throw new CouldNotCreateBlobException();

                contentsId = blobContents.Id;
            }
            catch(Exception ex)
            {
                throw new CouldNotCreateBlobVersionException("Blobs.CreateBlobVersion", ex);
            }

            var blobVersion = new BlobVersion
            {
                VersionId = Guid.NewGuid(),
                CreatedBy = creatorId,
                CreatedOn = DateTime.Now,
                ValidFrom = DateTime.Now,
                LastModifiedBy = creatorId,
                LastModified = DateTime.Now,
                BlobContentsId = contentsId,
                Etag = etag,
                ContentLength = contents.Length,
                ContentType = mimeType,
                FileName = fileName,
                BlobId = existingBlob.Id,
                BlobVariantHash = string.Empty
            };

            try
            {
                blobVersion = _blobRepository.CreateBlobVersion(blobVersion);

                if (blobVersion == null)
                    throw new CouldNotCreateBlobVersionException();
            }
            catch(Exception ex)
            {
                throw new CouldNotCreateBlobVersionException("Blobs.CreateBlobVersion", ex);
            }

            return blobVersion.VersionId;
        }

        public bool DeleteBlobVersion(Guid id, Guid versionId)
        {
            try
            {
                var blob = _blobRepository.GetBlobVersion(versionId);

                if (blob == null)
                    throw new BlobNotFoundException();

                return _blobRepository.DeleteBlobVersion(blob);
            }
            catch(Exception ex)
            {
                throw new CouldNotDeleteBlobException("Blobs.DeleteBlobVersion", ex);
            }
        }
    }
}
