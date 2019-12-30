======================================================================
NeoFace Cloud Type A �T���v��C#�A�v�� �\�[�X�R�[�h
======================================================================

������
NeoFace Cloud Type A (�摜�]���^)��REST API�̎g�p��ł��B
�摜�t�@�C�����w�肵�Ċ�F�؂����{���܂��B


���r���h���@

1.�W�J
  NeoFaceCloudSample�t�H���_��C�ӂ̏ꏊ�ɓW�J���܂��B
  
2. �v���W�F�N�g���J��
  Visual Studio 2017 ���g�p����NeoFaceCloudSample.sln���J���܂��B

3. API�L�[��ݒ�
  NfcRestApi.cs���J���A�e�i���gID��API�L�[�������p�̊��̂��̂ɏC�����܂��B
  
--------------------------------------------------------------------
    public static class Constants
    {
        public const string TenantID = "�e�i���gID���L��";
        public const string ManageApiKey = "API�L�[(�Ǘ��p)���L��";
        public const string AuthApiKey = "API�L�[(��F�ؗp)���L��";
        public const string NfcBaseUri = @"https://api.cloud.nec.com/neoface/";
        public const string NfcManageUri = NfcBaseUri + @"v1/";
        public const string NfcAuthUri = NfcBaseUri + @"f-face-image/v1/action/auth";
    }
--------------------------------------------------------------------

4. �r���h
  Visual Studio 2017�ɂ��r���h���܂��B


���A�v���̗��p���@
�R�}���h�v�����v�g�����s���܂��B
�W���G���[�o�͂ɁA���s����REST API�̃��N�G�X�g�ƃ��X�|���X���o�͂���A
�W���o�͂Ɍ��ʂ��o�͂���܂��B

[�o�^]
NeoFaceCloudSample.exe register JpegFile UserID UserName
UserID��UserName�̓��e�Ŋ�F�ؓo�^�Ώێ҂�o�^���AJpegFile�̊�摜��o�^���܂��B

<���s��>
>NeoFaceCloudSample.exe register master.jpg nec001 ���d���Y
��F�ؑΏێ҂�o�^���܂��� (userOId:1)


[1:1�F��]
NeoFaceCloudSample.exe auth JpegFile UserID
�w�肵��UserID�̊�F�ؓo�^�Ώێ҂ɑ΂��āAJpegFile�̊�摜�ŔF�؂��܂��B

<���s��>
>NeoFaceCloudSample.exe auth test1.jpeg nec001
��F�؂ɐ������܂��� (���[�U��:���d���Y �ƍ��X�R�A:0.902)


[1:N�F��]
NeoFaceCloudSample.exe auth JpegFile
JpegFile�̊�摜�ŔF�؂��܂��B

<���s��>
>NeoFaceCloudSample.exe auth test2.jpeg
��F�؂ɐ������܂��� (���[�U��:���d���Y �ƍ��X�R�A:0.894)


���g�p���C�u����
�ȉ���NuGet�p�b�P�[�W���g�p���Ă��܂��B
(�����I�Ƀ_�E�����[�h����܂��B)

�ENewtonsoft.Json
  ���C�Z���X�t�@�C��: packages\Newtonsoft.Json.10.0.3\LICENSE.md


�����C�Z���X
�{�\�[�X�R�[�h�́ACC0 1.0 �S���E �̂��ƂŒ񋟂���܂��B

CC0 1.0 �S���E
http://creativecommons.org/publicdomain/zero/1.0/deed.ja


���X�V����

2018/04/10 Version 1.0
  ����

2018/06/14 Version 1.0.0.1
  paramSetId�̌����C��
