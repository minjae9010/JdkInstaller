# JavaInstaller
JdkInstaller to gui and setting jdk Env Path


# JavaInstaller 프로그램 설명서

JavaInstaller는 Java Development Kit (JDK)를 설치하고 관리하기 위한 윈도우 응용 프로그램입니다. 이 프로그램을 사용하여 다양한 JDK 버전을 선택하고 설치할 수 있으며, 기존에 설치된 JDK를 확인하고 필요에 따라 삭제할 수도 있습니다.

## 주요 기능
JavaInstaller는 다음과 같은 주요 기능을 제공합니다:

1. JDK 설치
   - "자바설치" 버튼을 클릭하여 JDK를 설치할 수 있습니다.
   - JDK 버전, 제공자, 설치 가능한 버전을 선택하고 "설치" 버튼을 클릭하면 선택한 JDK가 설치됩니다.
   - 설치 과정은 진행 상태 바(`ProgressBar`)를 통해 시각적으로 표시됩니다.

2. JDK 목록 표시
   - 설치된 JDK의 목록은 메인 창(`MainWindow`)에 표시됩니다.
   - 각 JDK 항목은 제품 이름, JDK 버전, 배포 버전으로 구성되어 있습니다.
   - JDK 목록은 `ListView`를 사용하여 표시되며, 스타일이 적용되어 보기 좋게 표시됩니다.
   - JDK 항목 옆에는 "Apply", "Open Folder", "Delete" 버튼이 있어 각각 JDK를 적용, 설치 폴더 열기, 삭제할 수 있습니다.

3. JDK 삭제
   - JDK 목록에서 특정 JDK를 선택하고 "Delete" 버튼을 클릭하여 JDK를 삭제할 수 있습니다.
   - 선택한 JDK와 관련된 정보가 데이터베이스에서 삭제됩니다.

## 사용 방법
JavaInstaller를 사용하여 JDK를 설치하고 관리하는 방법은 다음과 같습니다:

1. JDK 설치
   - 메인 창에서 "자바설치" 버튼을 클릭합니다.
   - JDK 버전을 선택하기 위해 "JDK 버전" 콤보 박스에서 원하는 버전을 선택합니다.
   - 제공자를 선택하기 위해 "제공자" 콤보 박스에서 원하는 제공자를 선택합니다.
   - 설치 가능한 버전을 선택하기 위해 "설치 가능한 버전" 콤보 박스에서 원하는 버전을 선택합니다.
   - 선택한 JDK 버전, 제공자, 설치 가능한 버전이 올바르게 표시되는지 확인합니다.
   - "설치" 버튼을 클릭하여 선택한 JDK를 설치합니다.

2. JDK 목록 확인
   - 설치된 JDK 목록은 메인 창에서 확인할 수 있습니다.
   - 목록에는 JDK의 제품 이름, JDK 버전, 배포 버전이 표시됩니다.
   - 목록에서 특정 JDK를 선택하여 해당 JDK에 대한 작업을 수행할 수 있습니다.

3. JDK 삭제


   - JDK 목록에서 삭제할 JDK를 선택합니다.
   - 선택한 JDK와 관련된 정보를 삭제하기 위해 "Delete" 버튼을 클릭합니다.
   - 삭제한 JDK는 목록에서 사라집니다.

## 시스템 요구 사항
JavaInstaller를 실행하기 위해서는 다음과 같은 시스템 요구 사항이 필요합니다:

- Windows 운영 체제 (Windows 7 이상)
- .NET Framework 4.5 이상이 설치된 환경

JavaInstaller는 JDK를 설치하고 관리하기 위한 사용자 친화적인 인터페이스를 제공하여 Java 개발 환경 구축을 도와줍니다. 간편한 설치 과정과 직관적인 목록 관리 기능을 통해 JDK를 쉽게 관리할 수 있습니다.
